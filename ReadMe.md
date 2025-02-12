# 打Tag傻瓜工具

## 简要说明

1. 首次启动需要选择git工程目录，只要是打Tag的git目标工程就行，因为查询版本号和打Tag要环境。
2. 查询或打Tag不依赖工程当前分支，可以随意切工具指定地区打Tag。
3. 在tags_tool_config.json可以扩展地区、默认分支、打包类型等。
4. 有异常可以查看~tags_tool_log.txt日志。
5. 可以查看工具的缓存信息：%userprofile%\appdata\local\TagsTool。
6. 因为工具有些基于工程的git操作，所以要避免使用工具同时操作工程的git push pull等。

---

## 一些原理

1. 拉取所有分支和tag信息，不依赖工程：`git ls-remote {Config.GitURL}`
2. 拉取tag的详细信息，因为都是轻量标签，不包含提交信息，所以要查对应引用的提交：`git -c i18n.logOutputEncoding=GBK log -1 --format=\"%ad%n%an%n%s\" --date=format:\"%Y-%m-%d %H:%M:%S\" {tagName}`，其中`%ad` 是引用的提交记录的时间，`%an 是引用的提交记录的作者，`%s`是提交记录的日志。
3. 拉取分支对应版本，直接读对应分支版本文件：`git show origin/{CurBranch}:server/etc/server_version.txt`
4. 提交tag：
   1. 先拉取一下对应分支：`git fetch origin {CurBranch}`
   2. 创建tag：`git tag {CurTag} origin/{CurBranch}`
   3. push tag到远端：`git push origin {CurTag}`