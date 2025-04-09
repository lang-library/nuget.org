#include "dllmain.h"

#include <QtCore>
#include "strconvEx2.h"

#include <windows.h>
extern IMAGE_DOS_HEADER __ImageBase;

const QString ModulePath()
{
    static thread_local wchar_t fileName[_MAX_PATH];
    ::GetModuleFileNameW((HMODULE)&__ImageBase, fileName, sizeof(fileName));
    return QString::fromWCharArray(fileName);
}

class MyQt
{
public:
    QCoreApplication *app = nullptr;
    explicit MyQt()
    {
        std::vector<char *>argv;
        argv = get_ansi_args();
        int argc = argv.size();
        this->app = new QCoreApplication(argc, &argv[0]);
        //qDebug() << "MyQt initialized.";
        //qDebug() << qApp->arguments();
    }
    virtual ~MyQt()
    {
        delete this->app;
    }
};

MyQt myQt;
