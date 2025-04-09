#include <string>

class __SquareC {
public:
  __SquareC(double x, double y);
  double area();
  std::wstring greeting(const std::wstring &name);
  int execute(const std::wstring &exe, const std::wstring &args, const std::wstring &cwd);
private:
  double height_;
  double width_;
};
