using CSLisp.Core;
using CSLisp.Data;
using System.Collections.Generic;
using System.IO;

namespace CSLisp.Libs
{
    /// <summary>
    /// Manages standard libraries
    /// </summary>
    public class Libraries
    {
        /// <summary> All libraries as a list </summary>
        private static List<byte[]> GetAllBuiltInLibraries () =>
            new List<byte[]>() { Resources.Core, Resources.Record, Resources.User };

        /// <summary> Loads all standard libraries into an initialized machine instance </summary>
        public static void LoadStandardLibraries (Context ctx) {
#if false
            var allLibs = GetAllBuiltInLibraries();
            foreach (byte[] libBytes in allLibs) {
                using var stream = new MemoryStream(libBytes);
                using var reader = new StreamReader(stream);
                string libText = reader.ReadToEnd();
                LoadLibrary(ctx, libText);
            }
#else
            LoadLibrary(ctx, core_lisp);
            LoadLibrary(ctx, record_lisp);
            LoadLibrary(ctx, user_lisp);
#endif
        }

        /// <summary> Loads a single string into the execution context </summary>
        private static void LoadLibrary (Context ctx, string lib) {
            ctx.parser.AddString(lib);

            while (true) {
                Val result = ctx.parser.ParseNext();
                if (Val.Equals(Parser.EOF, result)) {
                    break;
                }

                Closure cl = ctx.compiler.Compile(result).closure;
                Val _ = ctx.vm.Execute(cl);
                // and we drop the output on the floor... for now... :)
            }
        }
        private static string core_lisp = """
            (package-set "core")

            (package-export 
            	'(let let* letrec define 
            	  and or cond case
            	  for dotimes

            	  first second third rest
            	  after-first after-second after-third
            	  fold-left fold-right reverse
            	  chain chain-list
            	  ))


            ;;
            ;;
            ;; MACROS

            ;; (let ((x 1) (y 2)) 
            ;;    (+ x 1)) 
            ;; => 
            ;; ((lambda (x y) (+ x y)) 1 2)
            ;;
            ;; e.g. (let ((x 1)) (+ x 1)) => 2
            ;;
            (defmacro let (bindings . body) 
            	`((lambda ,(map car bindings) ,@body) 
            		,@(map cadr bindings)))

            ;; (let* ((x 1) (y 2)) 
            ;;    (+ x y)) 
            ;; => 
            ;; (let ((x 1)) (let ((y 2)) (+ x y)))
            ;;
            (defmacro let* (bindings . body)
            	(if (null? bindings)
            		`(begin ,@body)
            		`(let (,(car bindings))
            			(let* ,(cdr bindings) ,@body))))

            ;; (letrec ((x (lambda () y)) 
            ;;          (y 1)) 
            ;;   (x)) 
            ;; => 
            ;; (let ((x nil) (y nil)) 
            ;;   (set! x (lambda () y)) 
            ;;   (set! y 1) 
            ;;   (x))
            ;;
            (defmacro letrec (bindings . body)
            	`(let ,(map (lambda (v) (list (car v) nil)) bindings)
            		,@(map (lambda (v) `(set! ,@v)) bindings)
            		,@body))

            ;; (define foo 5) => (begin (set! foo 5) 'foo)
            ;; (define (foo x) 5) => (define foo (lambda (x) 5))
            ;;
            (defmacro define (name . body)
            	(if (atom? name)
            		`(begin (set! ,name ,@body) (quote ,name))
            		`(define ,(car name) 
            			(lambda ,(cdr name) ,@body))))

            ;; (and x) => x
            ;; (and x y) => (if x y #f)
            ;; (and x y z) => (if x (and y z) #f) => (if x (if y z) #f)
            ;;
            (defmacro and (first . rest)
            	(if (null? rest) 
            		(car (list first))
            		(if (= (length rest) 1)
            			`(if ,first ,@rest #f)
            			`(if ,first (and ,@rest) #f))))

            ;; (or x) => x
            ;; (or x y) => (if* x y)
            ;; (or x y z) => (if* x (or y z)) => (if* x (if* y z))
            ;;
            (defmacro or (first . rest) 
            	(if (null? rest) 
            		(car (list first))
            		(if (= (length rest) 1)
            			`(if* ,first ,@rest)
            			`(if* ,first (or ,@rest)))))

            ;; (cond ((= a b) 1) 
            ;;		 ((= a c) 2) 
            ;;		 3)
            ;; => 
            ;; (if (= a b) (begin 1) (if (= a c) (begin 2) (begin 3)))
            ;;
            (defmacro cond (first . rest)
            	(if (null? rest)
            		`(begin ,first)
            		`(if ,(car first) 
            			(begin ,@(cdr first))
            			(cond ,@rest))))

            ;; (case (+ 1 2) 
            ;;		 (3 "foo") 
            ;;		 (4 "bar") 
            ;;		 "baz")
            ;; =>
            ;; (let (GENSYM-xxx (+ 1 2)) 
            ;;  	(cond ((= GENSYM-xxx 3) "foo")
            ;;			  ((= GENSYM-xxx 4) "bar")
            ;;			  "baz"))
            ;;
            (defmacro case (key . rest)
            	(let* ((keyval (gensym "CASE")))
            		`(let ((,keyval ,key))
            			(cond
            			   ,@(map (lambda (elt) 
            								(if (cons? elt) 
            									(cons (list '= keyval (car elt)) (cdr elt))
            									elt))
            						rest)))))

            ;; (for (x 0 (< x 10) (+ x 1)) (trace x) ...)
            ;; =>
            ;; (let ((x 0)))
            ;;     (while (< x 10) 
            ;;         (trace x) ... 
            ;;         (set! x (+ x 1))))
            (defmacro for (test . body)
                (let ((varname (car test)) 
            	      (init-value (cadr test)) 
            		  (predicate (caddr test)) 
            		  (step-value (car (cdddr test))))
            	    `(let ((,varname ,init-value)) 
            		    (while ,predicate ,@body (set! ,varname ,step-value)))))

            ;; (dotimes (x 10) (trace x) ...)
            ;; =>
            ;; (for (x 0 (< x 10) (+ x 1)) (trace x) ...)
            (defmacro dotimes (pars . body) 
                (let ((varname (car pars))
            	      (count (cadr pars)))
                    `(for (,varname 0 (< ,varname ,count) (+ ,varname 1))
            		    ,@body)))

            ;; (apply + '(1 2))
            ;; =>
            ;; (+ 1 2)
            (defmacro apply (fn args) 
                (let ((arglist (eval args))) 
            	    `(,fn ,@arglist)))



            ;; 
            ;;
            ;; FUNCTIONS

            (define first car)
            (define second cadr)
            (define third caddr)
            (define rest cdr)
            (define after-first cdr)
            (define after-second cddr)
            (define after-third cdddr)

            ;; (fold-left cons '() '(1 2 3)) 
            ;; 			=> (((() . 1) . 2) . 3) 
            ;; because 	=> '(cons (cons (cons '() 1) 2) 3) 
            ;;
            (define (fold-left fn base lst)
            	(if (= (length lst) 0) 
            		base
            		(fold-left fn (fn base (car lst)) (cdr lst))))

            ;; (fold-right cons '() '(1 2 3)) 
            ;;			=> '(1 2 3) 
            ;; because 	=> '(cons 1 (cons 2 (cons 3 '())))
            ;;
            (define (fold-right fn base lst)
            	(if (= (length lst) 0) 
            		base
            		(fn (car lst) (fold-right fn base (cdr lst)))))

            ;; (zip '(1 2) '(a b)) 
            ;;   => '((1 a) (2 b))
            (define (zip a b)
            	(if (or (null? a) (null? b))
            		'()
            		(cons (list (car a) (car b)) (zip (cdr a) (cdr b)))))

            ;; (reverse '(1 2 3)) 
            ;;   => '(3 2 1)
            (define (reverse lst)
              	(define (helper lst result)
                	(if (null? lst)
                  		result
                  		(helper (cdr lst) (cons (car lst) result))))
              	(helper lst '()))


            ;; (index-of 'b '(a b c)) => 1
            ;; (index-of 'b '(1 2 3)) => ()
            (define (index-of elt lst)
            	(letrec ((helper (lambda (l e i) (cond 
            						((null? l) l)
            						((= e (first l)) i)
            						(helper (rest l) e (+ i 1))))))
            		(helper lst elt 0)))	



            ;;
            ;; MORE MACROS THAT DEPEND ON THE ABOVE

            ;; (chain-list (list first second third))
            ;; =>
            ;; (lambda (GENSYM-123) (third (second (first GENSYM-123))))
            ;; 
            ;; e.g. (chain-list (list cdr cdr car)) == caddr
            ;;
            (defmacro chain-list (lst)
                (let* ((var (gensym))
            	       (args (reverse (eval lst)))
            	       (bodytext (eval `(fold-right list ',var ',args))))
            	  `(lambda (,var) ,bodytext)))

            ;; (chain first second third)
            ;; =>
            ;; (lambda (GENSYM-123) (third (second (first GENSYM-123))))
            ;;
            ;; e.g. (chain cdr cdr car) => caddr
            ;;
            (defmacro chain (first . rest)
                `(chain-list ',(cons first rest)))



            ;;
            ;;
            ;; TODO INTEROP

            ;; (.. struct '(field1 field2)) 
            ;;		=> (deref (deref struct field1) field2)
            ;;
            ;;(define (.. obj params)
            ;;	(fold-left deref obj params))
            
            """;
        private static string record_lisp = """
            ;; Experimental record support
            ;; Inspired by SRFI-9



            ;;
            ;; Usage examples:

            ; (define-record-type point 
            ;   	(make-point x y) 
            ;   	point? 
            ;   	(x getx setx!) 
            ;   	(y gety))

            ; (set! p (make-point 1 2)) ; => point object
            ;
            ; (point? p)      ; => #t
            ; (point? '(a b)) ; => #f
            ;
            ; (getx p)     ; => 1
            ; (gety p)     ; => 2
            ;
            ; (setx! p 42) ; => 42
            ; (getx p)     ; => 42
            ;
            ; ; (sety! p 42) ; => error, this accessor is not defined




            ;;
            ;; Implementation


            ; A record is a vector with a specific structure

            ; 0: closure that returns its record type
            ; 1..n: field values for record fields


            ; A record type is also a vector with a specific structure

            ; 0: special pointer to record:record-type
            ; 1: type name (e.g. point)
            ; 2: list of n field names (e.g. '(x y))


            ;; First we define our meta-level record:record-type
            ;; that's also a record type that describes itself

            (package-set "record")

            (package-export '(
            	record?
            	define-record-type
            	))


            ;; Helpers that make new record types
            ;; record:record-type points to the singleton prototype

            (define (record-type-vector? v) 
            	(and (vector? v) (= (vector-length v) 3)))

            (define (make-record-type name field-name-list)
             	(let ((type (make-vector 3)))
             		; this line ensures that the 0th element will be the prototype
             		; this ensures that record types are always identifiable
             		(vector-set! type 0 (or record-type type))
             		;; record type name
             		(vector-set! type 1 name)
             		;; field names
             		(vector-set! type 2 field-name-list)
             		type))


            ;; our special singleton record type
            (define record-type (make-record-type 'record:record-type 0))

            (define (record-type? type) 
            	(and (record-type-vector? type)
            		(= (vector-get type 0) record-type)))

            (define (record-type-fields type) 
            	(vector-get type 2))

            (define (record-type-field-count type) 
            	(length (record-type-fields type)))

            (define (record-type-field-index type field-name)
            	(index-of field-name (record-type-fields type)))


            ;; Now we can start making new records

            (define (record? v)
            	(and (vector? v)
            		(>= (vector-length v) 1)
            		(closure? (vector-get v 0))
            		(record-type? ((vector-get v 0)))))

            (define (record-get-type rec) ((vector-get rec 0)))
            (define (record-type-equals? rec type) (= (record-get-type rec) type))
            (define (record-field-count rec) (- (vector-length rec) 1))

            (define (record-get rec i) (vector-get rec (+ i 1)))
            (define (record-set! rec i value) (vector-set! rec (+ i 1) value))

            (define (make-record type)
            	(letrec ((field-count (record-type-field-count type))
            			 (rec (make-vector (+ field-count 1))))
            		(vector-set! rec 0 (lambda () type))
            		rec))

            (define (make-record-filled type values)
            	(letrec ((rec (make-record type))
            			 (field-count (record-field-count rec)))
            		(dotimes (i field-count)
            			(record-set! rec i (nth values i)))
            		rec))



            ;;
            ;; Now the macro that actually lets us define 
            ;; a record with fields and accessors

            (define (make-getter-defs type args)
            	(map (lambda (def) 
            			(if (< (length def) 2)
            				'()
            				(letrec ((field-name (first def))
            					 	 (getter-name (second def))
            				 		 (index (+ 1 (record-type-field-index type field-name))))
            					`(define (,getter-name rec) (vector-get rec ,index)))))
            		args))

            (define (make-setter-defs type args)
            	(map (lambda (def)
            			(if (< (length def) 3) 
            				'()
            				(letrec ((field-name (first def))
            					 	 (setter-name (third def))
            				 		 (index (+ 1 (record-type-field-index type field-name))))
            					`(define (,setter-name rec val) (vector-set! rec ,index val)))))
            		args))


            (defmacro define-record-type (name constructor predicate . args)
            	(letrec ((constructor-name (first constructor))
            		 	 (fields (rest constructor))
            		  	 (type (make-record-type name fields)))

            		`(begin
            			; define the constructor
            			(define ,constructor (make-record-filled ,type (list ,@fields)))

            			; define the predicate
            			(define (,predicate rec) (and (record? rec) (record-type-equals? rec ,type)))

            			; define accessors
            			,@(make-getter-defs type args)
            			,@(make-setter-defs type args)

            			type)))



            ;; for testing

            ; (define-record-type point (make-point x y) point? (x getx setx!) (y gety))
            ; (set! p (make-point 1 2)) 
            ; p
            """;
        private static string user_lisp = """
            (package-set "user")

            (package-import "core")
            (package-import "record")
            """;
    }

}