#! /usr/bin/env bash
set -uvx
set -e
rm -rf *.dll
scoop install swig
swig -csharp -c++ -cppext cpp -outdir . QProcess_Win64.i
dll-gen QProcess_Win64.cpp
pro64 -s QProcess_Win64.pro
cp -p QProcess_Win64.mingw64.static.dll QProcess_Win64.dll
cp -p QProcess_Win64.dll ~/cmd/
