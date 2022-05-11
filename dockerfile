#copy httpd(apache server) from dockerhub  where 'httpd' is image, ':alpine' is tag and base is alias
FROM httpd:alpine as base
WORKDIR /usr/local/apache2/htdocs/
EXPOSE 80
EXPOSE 433
#copy file from html to htdocs to run it     
COPY ./html/ .
