NET STOP "LightBase Server Watch Dog"
ping -n 3 127.0.0.1 >NULL

NET STOP "LightBase Server"
ping -n 3 127.0.0.1 >NULL

NET START "LightBase Server Watch Dog"
ping -n 3 127.0.0.1 >NULL