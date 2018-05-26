# AliAutoDNS
基于阿里云的域名解析接口，实现本地动态IP的动态解析

# 当前版本
- V1.0.0

## 目的
- 因为个人家庭网络一般没有静态IP，即使域名解析到家庭网络IP地址，宽带运营商也会不定时分配其他IP地址。
- 此程序实现个人家庭网络的外网映射，将域名解析地址指向个人公网IP，实现通过域名访问家庭网络。路由器部分需要自己单独做端口转发。

## 实现原理
- 阿里云域名提供通过API添加修改解析记录的功能（其他域名服务实也应该支持）。程序访问外网，实时查询本地公网IP地址，并根据实际情况判断是否更换域名指向IP。

## 使用者
- 首先注册阿里云账户，购买阿里云域名，前往AccessKeys获取具有管理云解析(DNS)权限的AccessKeyId和AccessKeySecret；
- 下载[压缩文件](https://github.com/shunchuan/AliAutoDNS/blob/master/software/AliAutoDNS_1.0.0.rar)，解压到运行路径；
- 修改.config配置文件中的value值，根据个人实际情况修改；
- 一般修改```AccessKeyId```、```AccessKeySecret```、```SetDNSDomainName```、```SetDNSHostRecord```、```BootFromBoot```即可；
- 注意：如果你域名下的解析记录很多(成百上千，否则可忽略)，解析主机记录尽量写的与其他记录无重叠，目前只查询前500条过滤数据进行筛选。比如要解析abc记录，不要有500个以上的包含abc字段的主机记录存在。
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <!--获取本地公网IP的接口地址-->
    <add key="GetPublicNetIPUrl" value="https://www.woaihuaye.com/ip"/>
    <!--阿里云api接口地址 注意最后的/? 不要去掉-->
    <add key="AliAPIUrl" value="https://alidns.aliyuncs.com/?"/>
    <add key="AccessKeyId" value="xxx"/>
    <add key="AccessKeySecret" value="xxx"/>
    <!--需要设置的解析域名-->
    <add key="SetDNSDomainName" value="xxx.com"/>
    <!--需要设置的域名主机记录-->
    <add key="SetDNSHostRecord" value="xxx"/>
    <!--系统休眠时间 毫秒-->
    <add key="SystemSleepTime" value="3000"/>
    <!--域名解析TTL-->
    <add key="DomainNameTTL" value="120"/>
    <!--当前解析线路 默认 ，或参考 https://help.aliyun.com/document_detail/29807.html?spm=a2c4g.11186623.2.8.Pe58xG -->
    <add key="DNSLine" value="default"/>

    <!--是否开启刷新本地解析记录，如果开启，可防止程序运行期间解析记录在其他地方修改-->
    <add key="RefreshRun" value="true"/>
    <!--如果开启刷新本地解析记录，则多少个间隔刷新一次， 即刷新时间间隔为 RefreshTimes * SystemSleepTime-->
    <add key="RefreshTimes" value="20"/>
    <!--是否开机自启动-->
    <add key="BootFromBoot" value="false"/>
  </appSettings>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5"/>
  </startup>
</configuration>

```

## 开发者
- 项目难点在于获取公共参数和签名值，官方文档查看[签名机制](https://help.aliyun.com/document_detail/29747.html?spm=a2c4g.11186623.2.3.7Ak6kX)。注意的是参数字段排序要区分大小写，C#的```Dictionary.OrderBy```方法并不区分大小写。
- HMAC-SHA1加密有微软现有的类，只需引用```System.Security.Cryptography```即可，项目中详情可查看[这里](https://github.com/shunchuan/AliAutoDNS/blob/master/AliHelper/Signature/Methods/Encrypt.cs)
- 公共参数的获取其实有官方的demo，地址在[阿里云开发工具包（SDK）](https://develop.aliyun.com/tools/sdk?#/dotnet)核心库中，我这里未使用，自己写了一遍。只调用了阿里云域名解析的查询、添加、修改三个api，文档地址在[解析管理接口](https://help.aliyun.com/document_detail/29772.html?spm=a2c4g.11186623.6.613.9eNWLG)。
- 没有什么难的地方，希望对你们有用。
