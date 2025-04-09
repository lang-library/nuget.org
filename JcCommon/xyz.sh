#! /usr/bin/env bash
set -uvx
set -e
qmake6 -project -norecursive -t lib -o xyz.pro
echo CONFIG += dll >> xyz.pro
echo QMAKE_LIBS += -lgraphite2 -lbz2  -lrpcrt4 -lusp10 >> xyz.pro
echo DESTDIR = \$\$PWD >> xyz.pro
rm -rf build
pro64 -s xyz.pro
