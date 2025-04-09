#! /usr/bin/env bash
set -uvx
set -e
swig -csharp -c++ -cppext cpp -outdir . square.i
g++ -shared -o ~/cmd/square.dll -I$HOME/common/include square.cpp square_wrap.cpp --static
