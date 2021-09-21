import 'package:flutter/material.dart';
import 'package:kwapp/src/app.dart';

import 'src/utils/SharedPrefs.dart';

//void main() => runApp(MyApp());
Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await SharedPrefs().init();
  runApp(DashboardApp.run());
}

/* class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: LoginPage(title: 'Flutter Demo Home Page'),
    );
  }
} */

class Profile extends StatelessWidget {
  final logoutAction;
  final String name;

  Profile(this.logoutAction, this.name);

  @override
  Widget build(BuildContext context) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        Container(
          width: 150,
          height: 150,
          decoration: BoxDecoration(
            border: Border.all(color: Colors.blue, width: 4.0),
            shape: BoxShape.circle,
          ),
        ),
        SizedBox(height: 24.0),
        Text('Name: $name'),
        SizedBox(height: 48.0),
        ElevatedButton(
          onPressed: () {
            logoutAction();
          },
          child: Text('Logout'),
        ),
      ],
    );
  }
}
