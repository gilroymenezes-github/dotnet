import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:kwapp/src/utils/SharedPrefs.dart';
import 'package:kwapp/src/utils/third_party/adaptive_scaffold.dart';

import 'login.dart';

class HomePage extends StatefulWidget {
  final String route;
  final int index;
  final bool isSignedIn;
  final VoidCallback onSignOut;

  static const destinations = [
    'Dashboard',
    'Deals',
    'Sales Orders',
    'Projects',
    'Reports',
    'Customers',
    'Master Records'
  ];

  HomePage({
    this.route,
    @required this.index,
    @required this.isSignedIn,
    @required this.onSignOut,
  });

  @override
  _HomePageState createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  bool _isLoggedIn = false;
  int _pageIndex = 0;
  var destinations = [
    'Dashboard',
    'Deals',
    'Sales Orders',
    'Projects',
    'Reports',
    'Customers',
    'Master Records'
  ];

  @override
  void initState() {
    super.initState();

    _isLoggedIn = SharedPrefs().accessToken.isNotEmpty;

    var _accessToken = _getQueryParam(Uri.base.toString(), 'access_token');

    if (_accessToken != null) _saveToken(_accessToken);
  }

  @override
  Widget build(BuildContext context) {
    print('Is user signed in? $_isLoggedIn \n Route: ${widget.route}');

    var route = ModalRoute.of(context).settings.name;

    var args = ModalRoute.of(context).settings.arguments;

    print('Screen route $route args: $args');

    return AdaptiveScaffold(
      title: Text('PowerUnit'),
      actions: [
        Padding(
          padding: const EdgeInsets.all(8.0),
          child: TextButton(
            style: TextButton.styleFrom(primary: Colors.white),
            onPressed: () => _isLoggedIn ? _handleSignOut() : _handleSignIn(),
            child: Text(widget.isSignedIn ? 'Sign Out' : 'Sign In'),
          ),
        ),
      ],
      // currentIndex: _pageIndex,
      currentIndex: widget.index,
      destinations: [
        AdaptiveScaffoldDestination(
            title: destinations[0], icon: Icons.dashboard),
        AdaptiveScaffoldDestination(title: destinations[1], icon: Icons.pages),
        AdaptiveScaffoldDestination(title: destinations[2], icon: Icons.pages),
        AdaptiveScaffoldDestination(title: destinations[3], icon: Icons.pages),
        AdaptiveScaffoldDestination(title: destinations[4], icon: Icons.report),
        AdaptiveScaffoldDestination(title: destinations[5], icon: Icons.pages),
        AdaptiveScaffoldDestination(title: destinations[6], icon: Icons.pages),
      ],
      // body: _pageAtIndex(_pageIndex),
      body: _pageAtIndex(widget.index),
      onNavigationIndexChange: (newIndex) {
        /* setState(() {
          _pageIndex = newIndex;
        }); */
        Navigator.push(
            context,
            MaterialPageRoute(
                builder: (context) => HomePage(
                    index: widget.index,
                    isSignedIn: widget.isSignedIn,
                    onSignOut: widget.onSignOut)));
      },
    );
  }

  void _saveToken(String accessToken) {
    SharedPrefs().accessToken = accessToken;
  }

  void _handleSignIn() {
    Navigator.push(
      context,
      MaterialPageRoute(
          builder: (context) => LoginPage(title: 'Login to PowerUnit')),
    );
  }

  Future<void> _handleSignOut() async {
    var shouldSignOut = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        title: Text('Are you sure you want to sign out?'),
        actions: [
          TextButton(
            child: Text('No'),
            onPressed: () {
              Navigator.of(context).pop(false);
            },
          ),
          TextButton(
            child: Text('Yes'),
            onPressed: () {
              SharedPrefs().accessToken = '';
              Navigator.of(context).pop(true);
            },
          ),
        ],
      ),
    );

    if (!shouldSignOut) {
      return;
    }

    widget.onSignOut();
  }

  Widget _pageAtIndex(int index) {
    /* if (index == 0) {
      return DashboardPage();
    } */

    return Center(
        child: Text('${destinations[index]} page -- Route: ${widget.route}'));
  }

  String _getQueryParam(String url, String param) {
    var split = url.split('&');
    for (var cha in split) {
      if (cha.startsWith(param))
        return cha.substring(param.length + 1, cha.length);
    }

    return null;
  }
}
