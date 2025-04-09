#include <string>

class SquareC {
public:
  SquareC(double x, double y);
  double area();
  std::wstring greeting();
private:
  double height_;
  double width_;
};
