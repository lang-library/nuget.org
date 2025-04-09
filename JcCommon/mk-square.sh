#! /usr/bin/env bash
set -uvx
set -e
ts=`date "+_%Y_%m%d_%H%M_%S_"`
version=${ts//_/.}
echo $version
rm -f __*
echo %module __square${ts} > __square.i
cat square.i.txt >> __square.i
cat __square.i
swig -csharp -c++ -cppext cpp -outdir . __square.i

#g++ -shared -o ~/cmd/__square${ts}.dll -I$HOME/common/include square.cpp __square_wrap.cpp --static

qmake6 -project -norecursive -t lib -o __square$ts.pro
echo CONFIG += dll >> __square$ts.pro
echo INCLUDEPATH += \$\(HOME\)/common/include >> __square$ts.pro
echo QMAKE_LIBS += -lgraphite2 -lbz2  -lrpcrt4 -lusp10 >> __square$ts.pro
echo DESTDIR = \$\$PWD >> __square$ts.pro
rm -rf build
pro64 -s __square$ts.pro
cp -p __square$ts.dll ~/cmd/
#rm -f *.pro

cat << EOS > JcCommon.swig.cs
//css_inc __square${ts}.cs
//css_inc __square${ts}PINVOKE.cs
//css_native __square${ts}.dll
//css_inc __SquareC.cs
EOS

cat JcCommon.swig.cs
