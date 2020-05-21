@echo off
echo Executando pre-build...
type nul > app_offline.htm
git rev-parse --abbrev-ref HEAD > "git-info.txt"
git show --format="%%H%%n%%cn%%n%%cd" --quiet --date=format:"%%d/%%m/%%Y %%H:%%M" >> "git-info.txt"