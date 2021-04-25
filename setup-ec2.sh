#!bin/bash
# update apt repositories
sudo apt update -y && sudo apt upgrade -y

# install gui and vnc
sudo apt-get install xfce4 xfce4-goodies tigervnc-standalone-server -y
