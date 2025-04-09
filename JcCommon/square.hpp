#include <string>

class __SquareC {
public:
  __SquareC(double x, double y);
  double area();
  std::wstring greeting(const std::wstring &name);
private:
  double height_;
  double width_;
};
