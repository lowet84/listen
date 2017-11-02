FROM microsoft/aspnetcore:2.0.0

ADD /out /app
WORKDIR /app
EXPOSE 7000
CMD dotnet Listen.Api.dll
