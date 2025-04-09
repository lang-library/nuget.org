#include "square.hpp"
#include "strconv2.h"

__SquareC::__SquareC(double x, double y) : height_(x), width_(y) {
}

// 面積を返す
double __SquareC::area() {
  return height_ * width_;
}

std::wstring __SquareC::greeting() {
  //return L"helloハロー©";
  return utf8_to_wide(u8"u8ハロー©");
}
