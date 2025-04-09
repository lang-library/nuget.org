#! /usr/bin/env bash
set -uvx
set -e
swig -csharp -c++ -cppext cpp -outdir . __square.i
g++ -shared -o ~/cmd/__square.dll -I$HOME/common/include square.cpp __square_wrap.cpp --static
