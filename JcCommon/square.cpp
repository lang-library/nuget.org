#include "square.hpp"
#include "strconv2.h"

SquareC::SquareC(double x, double y) : height_(x), width_(y) {
}

// 面積を返す
double SquareC::area() {
  return height_ * width_;
}

std::wstring SquareC::greeting() {
  //return L"helloハロー©";
  return utf8_to_wide(u8"u8ハロー©");
}
