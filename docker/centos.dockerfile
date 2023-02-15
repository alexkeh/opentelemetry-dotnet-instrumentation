FROM ghcr.io/open-telemetry/opentelemetry-dotnet-instrumentation-centos7-build-image:main

RUN rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
RUN yum -y install dotnet-sdk-6.0 dotnet-sdk-7.0

WORKDIR /project
COPY ./docker-entrypoint.sh /
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["/bin/bash"]