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
### 2. Instalace DOTNET


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