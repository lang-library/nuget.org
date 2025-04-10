#! /usr/bin/env qc
QT = core
CONFIG += c++17 cmdline

TEMPLATE = lib
CONFIG += c++17 dll

INCLUDEPATH += $(HOME)/common/include
DESTDIR = $$PWD
DEFINES += STRCONV_NLOHMANN

TARGET = QProcess_Win64
TARGET = $${TARGET}.$${QMAKE_HOST.arch}
#message($$QMAKE_QMAKE)
contains(QMAKE_QMAKE, .*static.*) {
    message( "[STATIC BUILD]" )
    DEFINES += QT_STATIC_BUILD
    TARGET = $${TARGET}-static
} else {
    message( "[SHARED BUILD]" )
    TARGET = $${TARGET}-dynamic
}

RESOURCES += QProcess_Win64.qrc
SOURCES += D:\home09\nuget.org\nuget.org\QProcess.Win64\QProcess_Win64.cpp
SOURCES += D:\home09\nuget.org\nuget.org\QProcess.Win64\dllmain.cpp
SOURCES += D:\home09\nuget.org\nuget.org\QProcess.Win64\QProcess_Win64_wrap.cpp
