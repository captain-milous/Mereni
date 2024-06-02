# Mereni

Tato aplikace je určená získávání naaměřených hodnot pro Raspberry Pi

## Autor

Miloš Tesař

## Instalace

### 1. Update Linuxu
```
sudo apt-get update 

sudo apt-get upgrade
```
Pokud nemáte správné rozpoložení klávesnice:
```
sudo raspi-config
```
### 2. Instalace DOTNET
Přejděte na stránku [download .NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) a vyberte verzi pro ARM64 (pro Linux).

Vytvořte adresář:
```
sudo mkdir -p /usr/local/share/dotnet
```
Rozbalte SDK do /usr/local/share/dotnet:
```
sudo tar zxf dotnet-sdk-8.0.100-linux-arm64.tar.gz -C /usr/local/share/dotnet
```
Vytvoření symbolického odkazu:
```
sudo ln -s /usr/local/share/dotnet/dotnet /usr/local/bin/dotnet
```
Ověření symbolického odkazu:
```
ls -l /usr/local/bin/dotnet
```
Nastavení PATH pro všechny uživatele:
```
echo 'export DOTNET_ROOT=/usr/local/share/dotnet' | sudo tee -a /etc/profile
echo 'export PATH=$PATH:/usr/local/share/dotnet' | sudo tee -a /etc/profile
source /etc/profile
```
Restartujte shell nebo systém:
```
exec $SHELL
```
Ověření instalace:
```
sudo dotnet --version
```
### Nastavení statické ip adresy
```
sudo apt-get install dhcpcd5
```
```
sudo systemctl enable dhcpcd
sudo systemctl start dhcpcd
```
Otevřete soubor dhcpcd.conf:
```
sudo nano /etc/dhcpcd.conf
```
Přidejte následující řádky na konec souboru:
```
interface eth0
static ip_address=192.168.1.240/24
static routers=192.168.1.1
static domain_name_servers=192.168.1.1
```
*Pokud používáte WiFi, změňte eth0 na wlan0.*

Restartujte službu dhcpcd:
```
sudo systemctl restart dhcpcd
```
Zkontrolujte nastavení: Ujistěte se, že se statická IP adresa použila správně.
```
hostname -I
```
### Instalace aplikace

### Vytvoření sdílené složky