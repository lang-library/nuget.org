#include "square.hpp"
#include "strconvQt2.h"
#include <QtCore>
#include "process_util.h"
#include "wstrutil.h"

__SquareC::__SquareC(double x, double y) : height_(x), width_(y) {
}

// 面積を返す
double __SquareC::area() {
  return height_ * width_;
}

std::wstring __SquareC::greeting(const std::wstring &name) {
  //return L"helloハロー©";
  //return utf8_to_wide(u8"u8ハロー©");
  QString msg = QString("helloハロー© %1").arg(QString::fromStdWString(name));
  return qstr_to_wide(msg);
}

int __SquareC::execute(const std::wstring &exe, const std::wstring &args, const std::wstring &cwd)
{
    std::vector<std::wstring> split = strutil::split(args, std::wstring(L"\t"));
    QStringList arguments;
    for (std::size_t i=0; i<split.size(); i++)
    {
        qDebug() << i << wide_to_qstr(split[i]);
        arguments.append(wide_to_qstr(split[i]));
    }
    return ProcessUtil::run(false, wide_to_qstr(exe), arguments, wide_to_qstr(cwd));
}

