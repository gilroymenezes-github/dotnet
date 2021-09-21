import 'dart:async';
import 'package:http/http.dart' as http;
import 'api.dart';

class RemoteDashboardApi implements DashboardApi {
  @override
  LoginApi get loginApi => RemoteLoginApi();
}

class RemoteLoginApi implements LoginApi {
  @override
  Future login() async {
    final response = await http.get(
        Uri.https('run.mocky.io', 'v3/4ed32188-fd23-49db-8f32-fa1dc773c10a'));

    if (response.statusCode == 200) {
      // If the server did return a 200 OK response,
      // then parse the JSON.
      return null;
    } else {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to load album');
    }
  }
}
