import 'package:shared_preferences/shared_preferences.dart';
import 'constants.dart';

class SharedPrefs {
  static SharedPreferences _sharedPrefs;

  factory SharedPrefs() => SharedPrefs._internal();

  SharedPrefs._internal();

  Future<void> init() async {
    _sharedPrefs ??= await SharedPreferences.getInstance();
  }

  String get accessToken => _sharedPrefs.getString(access_token) ?? "";

  set accessToken(String accessToken) =>
      _sharedPrefs.setString(access_token, accessToken);
}
