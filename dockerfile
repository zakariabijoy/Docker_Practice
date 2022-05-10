#copy httpd(apache server) from dockerhub  where 'httpd' is image, ':alpine' is tag and base is alias
FROM httpd:alpine as base

#copy file from html to htdocs to run it     
COPY ./html/ /usr/local/apache2/htdocs/

# new httpd image is pulled 
FROM httpd as final
#copy file from previous httpd iimae to new httpd image to run it 
COPY --from=base /usr/local/apache2/htdocs/ /app/     