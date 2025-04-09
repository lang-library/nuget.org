#include <string>

class __SquareC {
public:
  __SquareC(double x, double y);
  double area();
  std::wstring greeting();
private:
  double height_;
  double width_;
};
