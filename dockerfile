#copy httpd(apache server) from dockerhub 
from httpd:alpine

#copy file from html to htdocs to run it     
copy ./html/ /usr/local/apache2/htdocs/    