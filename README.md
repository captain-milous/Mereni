# Mereni

Tento projekt je jednoduchý HTTP listener, který přijímá GET požadavky s určitými parametry, a zapisuje tyto parametry do CSV souborů.
Tato aplikace je určená získávání naměřených hodnot z měřících zařízeních pro Raspberry Pi pomocí http get requestu.

## Autor

Miloš Tesař

## Struktura programu

- **Program.cs**: Hlavní třída obsahující vstupní bod programu a HTTP listener.
- **CSVHandler.cs**: Třída pro práci s CSV soubory, obsahuje metodu pro ukládání dat do CSV.

## Instalace

### 1. Update Linuxu
```
sudo apt-get update 

sudo apt-get upgrade -y
```
Pokud nemáte správné rozpoložení klávesnice:
```
sudo raspi-config
```


### 2. Instalace DOTNET

**Přejděte na stránku [download .NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) a vyberte verzi pro ARM64 (pro Linux).**

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


### 3. Instalace aplikace
Nainstalujte git:
```
sudo apt-get install git -y
```
Naklonujte repositoř:
```
git clone https://github.com/captain-milous/Mereni.git
```
Vytvořte složku pro Zaznamenávání měření:
```
cd Mereni/bin/Debug/net8.0/

sudo mkdir Mereni

sudo chmod 777 Mereni
```
#### Spuštění aplikace:
```
sudo dontnet Mereni.dll
```


### 4. Nastavení statické ip adresy
```
sudo apt-get install dhcpcd5 -y  
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


### 5. Vytvoření sdílené složky
Nainstalujte balíček Samba:
```
sudo apt install samba -y
```
Otevřete konfigurační soubor Samba:
```
sudo nano /etc/samba/smb.conf
```
Přidejte na konec souboru následující konfiguraci pro sdílenou složku:
```
[shared]
path = /home/username/Mereni/bin/Debug/net8.0/Mereni
writeable = yes
create mask = 0777
directory mask = 0777
public = yes
```
Nastavte správná oprávnění:
```
sudo chown -R username:username /home/username/Mereni/bin/Debug/net8.0/Mereni
sudo chmod -R 0777 /home/username/Mereni/bin/Debug/net8.0/Mereni
```
Přidejte Samba uživatele:
```
sudo smbpasswd -a pi
```
Restartujte Samba, aby se změny projevily:
```
sudo systemctl restart smbd
```


### 6. Nastavení zapnutí aplikace po restartu
Otevřete cron konfiguraci pro uživatele root:
```
sudo crontab -e

```
Přidejte následující řádek na konec souboru:
```
@reboot /usr/bin/sudo /usr/bin/dotnet /cesta/k/vasi/aplikaci/myapp.dll
```


## Připojení ke sdílené složce

1. Otevřete Průzkumník souborů na Windows 11.

2. Klikněte na „Tento počítač“.

3. Klikněte na „Připojit síťovou jednotku“ v horní nabídce.

4. Vyberte písmeno jednotky a zadejte adresu vaší sdílené složky ve formátu:
```
\\IP_adresa_Raspberry_Pi\shared
```
5. Klikněte na „Dokončit“.

6. Zadejte uživatelské jméno a heslo, které jste nastavili pro Samba (např. uživatel: pi, heslo: vámi_zadané_heslo).