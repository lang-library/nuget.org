//+source dllmain.cpp
//+source QProcess_Win64_wrap.cpp
#include "QProcess_Win64.hpp"
#include <QtCore>
#include "strconvQt2.h"
#include "process_util.h"
#include "wstrutil.h"

int Execute3(const std::wstring &exe, const std::wstring &args, const std::wstring &cwd)
{
    std::vector<std::wstring> split = strutil::split(args, std::wstring(L"\t"));
    QStringList arguments;
    for (std::size_t i=0; i<split.size(); i++)
    {
        //qDebug() << i << wide_to_qstr(split[i]);
        arguments.append(wide_to_qstr(split[i]));
    }
    return ProcessUtil::run(false, wide_to_qstr(exe), arguments, wide_to_qstr(cwd));
}

void Launch3(const std::wstring &exe, const std::wstring &args, const std::wstring &cwd)
{
    std::vector<std::wstring> split = strutil::split(args, std::wstring(L"\t"));
    QStringList arguments;
    for (std::size_t i=0; i<split.size(); i++)
    {
        //qDebug() << i << wide_to_qstr(split[i]);
        arguments.append(wide_to_qstr(split[i]));
    }
    ProcessUtil::launch(true, wide_to_qstr(exe), arguments, wide_to_qstr(cwd));
}
