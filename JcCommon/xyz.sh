#! /usr/bin/env bash
set -uvx
set -e
ts=`date "+_%Y_%m%d_%H%M_%S_"`
qmake6 -project -norecursive -t lib -o xyz$ts.pro
echo CONFIG += dll >> xyz$ts.pro
echo QMAKE_LIBS += -lgraphite2 -lbz2  -lrpcrt4 -lusp10 >> xyz$ts.pro
echo DESTDIR = \$\$PWD >> xyz$ts.pro
rm -rf build
pro64 -s xyz$ts.pro
rm -f *.pro
