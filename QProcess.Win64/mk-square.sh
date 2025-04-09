#! /usr/bin/env bash
set -uvx
set -e
ts=`date "+_%Y_%m%d_%H%M_%S_"`
version=${ts//_/.}
echo $version

swig -csharp -c++ -cppext cpp -outdir . __square.i

qmake6 -project -norecursive -t lib -o __square.pro
echo CONFIG += dll >> __square.pro
echo INCLUDEPATH += \$\(HOME\)/common/include >> __square.pro
echo QMAKE_LIBS += -lgraphite2 -lbz2  -lrpcrt4 -lusp10 >> __square.pro
echo DESTDIR = \$\$PWD >> __square.pro
rm -rf build
pro64 -s __square.pro
