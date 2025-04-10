#! /usr/bin/env bash
set -uvx
set -e
swig -csharp -c++ -cppext cpp -outdir . QProcess_Win64.i
qmake6 -project -norecursive -t lib -o QProcess_Win64.pro
echo CONFIG += dll >> QProcess_Win64.pro
echo INCLUDEPATH += \$\(HOME\)/common/include >> QProcess_Win64.pro
echo QMAKE_LIBS += -lgraphite2 -lbz2  -lrpcrt4 -lusp10 >> QProcess_Win64.pro
echo DESTDIR = \$\$PWD >> QProcess_Win64.pro
rm -rf build
pro64 -s QProcess_Win64.pro
cp QProcess_Win64.dll ~/cmd/
