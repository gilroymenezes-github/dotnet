abstract class Auth {
  Future<bool> get isSignedIn;
  Future<User> signIn();
  Future signOut();
}

abstract class User {
  String get uid;
}

class SignInException implements Exception {}
