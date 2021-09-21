import 'dart:math';
import 'package:shared_preferences/shared_preferences.dart';

import 'auth.dart';

class RemoteAuthService implements Auth {
  //Future<bool> get isSignedIn async => false;
  Future<bool> get isSignedIn async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    var refreshToken = prefs.getString('token');

    print('Shared Prefs: Token: $refreshToken');

    return refreshToken.isNotEmpty;
  }

  @override
  Future<User> signIn() async {
    var random = Random();
    if (random.nextInt(4) == 0) {
      throw SignInException();
    }
    return MockUser();
  }

  @override
  Future signOut() async {
    return null;
  }
}

class MockUser implements User {
  String get uid => "123";
}
