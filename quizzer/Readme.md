## Connection String ## 

OFFKEY: mongodb 
START: $ mongodb --config /usr/local/etc/mongo.conf &

Configuration file has bindIp which uses current ifconfig value of OFFKEY

Connection for Robo 3T using SSH

Connection for application for mongodb://localhost:27107 possible when SSH tunnel
windows10> ssh -L 27017:localhost:27017 offkey

Firewall check from nofi 
$ nc -zv 192.168.0.101 27017
