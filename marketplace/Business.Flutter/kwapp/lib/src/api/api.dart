abstract class DashboardApi {
  LoginApi get loginApi;
}

abstract class LoginApi {
  Future login();
}
