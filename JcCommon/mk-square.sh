#! /usr/bin/env bash
set -uvx
set -e
ts=`date "+_%Y_%m%d_%H%M_%S_"`
rm __*
echo %module __square$ts > __square.i
cat square.i.txt >> __square.i
cat __square.i
swig -csharp -c++ -cppext cpp -outdir . __square.i
g++ -shared -o ~/cmd/__square$ts.dll -I$HOME/common/include square.cpp __square_wrap.cpp --static
