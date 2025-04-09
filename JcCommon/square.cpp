#include "square.hpp"
#include "strconvQt2.h"
#include <QtCore>

__SquareC::__SquareC(double x, double y) : height_(x), width_(y) {
}

// 面積を返す
double __SquareC::area() {
  return height_ * width_;
}

std::wstring __SquareC::greeting(const std::wstring &name) {
  //return L"helloハロー©";
  //return utf8_to_wide(u8"u8ハロー©");
  QString msg = QString("helloハロー© %1").arg(QString::fromStdWString(name));
  return qstr_to_wide(msg);
}
