import 'dart:convert';

import 'package:flutter/material.dart';

import 'package:openid_client/openid_client_browser.dart';
import 'package:shared_preferences/shared_preferences.dart';

class LoginPage extends StatefulWidget {
  LoginPage({Key key, this.title}) : super(key: key);
  final String title;

  @override
  _LoginPageState createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  bool isBusy = false;
  bool isLoggedIn = false;
  String errorMessage;
  String name;
  Authenticator authenticator;
  TokenResponse tokenResponse;
  UserInfo userInfo;
  Credential credential;

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'PowerUnit Kwapp',
      home: Scaffold(
        appBar: AppBar(
          title: Text('PowerUnit Auth'),
        ),
        body: Center(
          child: isBusy
              ? CircularProgressIndicator()
              : Login(loginAction, errorMessage),

          /// take to dashboard from here or remove this page
          /* isBusy
              ? CircularProgressIndicator()
              : isLoggedIn
                  ? Profile(logoutAction, name)
                  : Login(loginAction, errorMessage), */
        ),
      ),
    );
  }

  Map<String, dynamic> parseIdToken(String idToken) {
    final parts = idToken.split(r'.');
    assert(parts.length == 3);
    return jsonDecode(
        utf8.decode(base64Url.decode(base64Url.normalize(parts[1]))));
  }

  Future<void> loginAction() async {
    setState(() {
      isBusy = true;
      errorMessage = '';
    });

    SharedPreferences prefs = await SharedPreferences.getInstance();

    try {
      // create the client
      final uri = new Uri(scheme: "https", host: "pu-auth.azurewebsites.net");
      final issuer = await Issuer.discover(uri);
      final client = new Client(issuer, "business.flutter.local");

      // create an authenticator
      var a = new Authenticator(client, scopes: ["openid", "profile", "email"]);

      //a.authorize();
      var c = await a.credential;

      if (c == null) {
        a.authorize();
      } else {
        final t = await c.getTokenResponse();
        final u = await c.getUserInfo();

        prefs.setString('refresh_token', tokenResponse.refreshToken);
        prefs.setString('user', name);

        print('User: ${u.name} ${u.email}, Token: ${t.refreshToken}');

        setState(() {
          isBusy = false;
          isLoggedIn = true;
          authenticator = a;
          name = u.email;
          userInfo = u;
          tokenResponse = t;
          credential = c;
        });
      }
    } catch (e, s) {
      print('login error: $e - stack $s');
      setState(() {
        isBusy = false;
        isLoggedIn = false;
        errorMessage = e.toString();
      });
    }
  }

  void logoutAction() async {
    //authenticator.logout(); // this does strange things when enabled..
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.setString('refresh_token', '');
    await prefs.setString('user', '');
    setState(() {
      isLoggedIn = false;
      isBusy = false;
    });
  }

  @override
  initState() {
    //initAction();
    loginAction();
    super.initState();
  }

  void initAction() async {
    try {
      final idToken = Uri.base.toString().substring(
          'http://localhost:5020/#id_token='.length,
          Uri.base.toString().length);
      if (idToken == null) return;

      setState(() {
        isBusy = false;
        isLoggedIn = true;
      });
    } catch (e, s) {
      print('error on refresh token: $e - stack $s');
      logoutAction();
    }
  }

  /* void _callApi() async {
    var url = 'http://192.168.0.100:5010/currencies';
    var access_token = tokenResponse.accessToken;

    var response =
        await http.get(url, headers: {"Authorization": "Bearer $access_token"});
    var body = response.body;
    var a = "";
  } */
}

class Login extends StatelessWidget {
  final loginAction;
  final String loginError;

  const Login(this.loginAction, this.loginError);

  @override
  Widget build(BuildContext context) {
    loginAction();

    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        Text('Redirecting to Login Page'),
        CircularProgressIndicator(),
        Text(loginError ?? ''),
      ],
    );
  }
}
