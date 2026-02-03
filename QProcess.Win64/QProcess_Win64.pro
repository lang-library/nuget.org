#! /usr/bin/env qc
QT = core
TEMPLATE = lib
CONFIG += c++20 dll

QMAKE_CXXFLAGS += -w
INCLUDEPATH += $(HOME)/common/include
INCLUDEPATH += $(HOME)/cmd
DESTDIR = $$PWD
DEFINES += STRCONV_NLOHMANN
LIBS += -lgccpp -lgc -lpthread
#LIBS += $(HOME)/cmd/xxx.native.lib

#INCLUDEPATH += $(HOME)/cmd/neko-2.3.0-win64/include
#LIBS += $(HOME)/cmd/neko-2.3.0-win64/neko.lib

DESTDIR = $$PWD
DEFINES += STRCONV_NLOHMANN

TARGET = QProcess_Win64
MSYSNM = $$lower($$(MSYSTEM))
TARGET = $${TARGET}.$${MSYSNM}
#message($$QMAKE_QMAKE)
contains(QMAKE_QMAKE, .*static.*) {
    message( "[STATIC BUILD]" )
    DEFINES += QT_STATIC_BUILD
    TARGET = $${TARGET}.static
} else {
    message( "[SHARED BUILD]" )
    TARGET = $${TARGET}.dynamic
}

RESOURCES += QProcess_Win64.qrc
SOURCES += QProcess_Win64.cpp
SOURCES += dllmain.cpp
SOURCES += QProcess_Win64_wrap.cpp
