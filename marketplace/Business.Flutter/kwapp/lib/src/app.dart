import 'package:flutter/material.dart';
import 'package:kwapp/src/utils/SharedPrefs.dart';
import 'package:provider/provider.dart';

import 'api/remote.dart';
import 'api/api.dart';
import 'auth/auth.dart';
import 'auth/RemoteAuthService.dart';
import 'pages/home.dart';
import 'pages/sign_in.dart';

///https://stackoverflow.com/questions/61850247/flutter-web-navigation-with-persistent-drawer-and-appbar

class AppState {
  final Auth auth;
  DashboardApi api;

  AppState(this.auth);
}

typedef DashboardApi ApiBuilder(User user);

class DashboardApp extends StatefulWidget {
  static ApiBuilder _remoteApiBuilder = (user) => RemoteDashboardApi();

  final Auth auth;
  final ApiBuilder apiBuilder;

  DashboardApp.run()
      : auth = RemoteAuthService(),
        apiBuilder = _remoteApiBuilder;

  @override
  _DashboardAppState createState() => _DashboardAppState();
}

class _DashboardAppState extends State<DashboardApp> {
  AppState _appState;
  String user = '';
  bool _isLoggedIn = false;

  void initState() {
    getLoggedInState();

    super.initState();
    _appState = AppState(widget.auth);
  }

  @override
  Widget build(BuildContext context) {
    return Provider.value(
      value: _appState,
      child: MaterialApp(
        ///https://flutter.dev/docs/cookbook/navigation/named-routes
        /* home: SignInSwitcher(
          appState: _appState,
          apiBuilder: widget.apiBuilder,
        ), */
        initialRoute: '/',
        routes: {
          '/': (context) => HomePage(
              index: 0, isSignedIn: _isLoggedIn, onSignOut: _handleSignOut),
          '/Deals': (context) => HomePage(
              route: '/Deals',
              index: 1,
              isSignedIn: _isLoggedIn,
              onSignOut: _handleSignOut),
          '/SalesOrders': (context) => HomePage(
              route: '/SalesOrders',
              index: 2,
              isSignedIn: _isLoggedIn,
              onSignOut: _handleSignOut),
        },
      ),
    );
  }

  void getLoggedInState() {
    if (SharedPrefs() == null) print('SharedPrefs() is null');

    _isLoggedIn = SharedPrefs().accessToken.isNotEmpty;
    //_isLoggedIn = true;
  }

  Future _handleSignOut() async {
    //SharedPrefs().accessToken = '';
    await _appState.auth.signOut();
  }
}

class SignInSwitcher extends StatefulWidget {
  final AppState appState;
  final ApiBuilder apiBuilder;

  SignInSwitcher({
    this.appState,
    this.apiBuilder,
  });

  @override
  _SignInSwitcherState createState() => _SignInSwitcherState();
}

class _SignInSwitcherState extends State<SignInSwitcher> {
  bool _isSignedIn = false;
  String user = '';

  @override
  Widget build(BuildContext context) {
    return AnimatedSwitcher(
      switchInCurve: Curves.easeOut,
      switchOutCurve: Curves.easeIn,
      duration: Duration(milliseconds: 200),
      child: HomePage(
        isSignedIn: _isSignedIn,
        onSignOut: _handleSignOut,
      ),
      /* child: _isSignedIn
          ? HomePage(
              isSignedIn: _isSignedIn,
              onSignOut: _handleSignOut,
            )
          : SignInPage(auth: widget.appState.auth, onSuccess: _handleSignIn), */
    );
  }

  void _isLoggedIn() async {
    setState(() async {
      _isSignedIn = await widget.appState.auth.isSignedIn;
    });
  }

  void _handleSignIn(User user) {
    widget.appState.api = widget.apiBuilder(user);

    setState(() {
      _isSignedIn = true;
    });
  }

  Future _handleSignOut() async {
    await widget.appState.auth.signOut();

    setState(() {
      _isSignedIn = false;
    });
  }
}
