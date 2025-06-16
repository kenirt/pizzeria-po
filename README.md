# System zamawiania pizzy

Aplikacja symuluje proces zamawiania pizzy – od wyboru składników lub gotowej kompozycji, przez przetwarzanie zamówienia, aż po jego realizację. System umożliwia zarządzanie kolejką zamówień oraz zmianę ich statusów.

Dodatkowo aplikacja zawiera funkcjonalności logowania i rejestracji użytkowników, co pozwala na personalizację zamówień oraz śledzenie historii zakupów.

## Wymagania

- .NET SDK 8.0  
- System operacyjny: Windows, Linux lub macOS  
- Git  

## Instalacja

Aby zainstalować aplikację, należy sklonować repozytorium za pomocą polecenia:

```bash
git clone git@gitlab.com:zemlo-borkowski/pizzeria-projekt-po.git
```
## Uruchomienie projektu

Aby uruchmoić projekt można urochomić plik "pizzeria.exe" który się znajduje `pizzeria-projekt-po\pizzeria\pizzeria\bin\Debug\net8.0` lub urochmić za pomocą konsoli
```bash
..../pizzeria.exe
```
## UI programu

Aplikacja działa w trybie konsolowym i oferuje intuicyjny interfejs użytkownika, który został zaprojektowany z myślą o łatwości użytkowania. Interfejs został zaimplementowany w folderze `/UI`, który zawiera wiele plików odpowiedzialnych za różne komponenty UI. 

## Logowanie/Rejestracja

Na początku urochmienia programu jesteśmy poproszeni o zalogowanie lub rejestrację, wciśnięcie innego przyciska wylączy program
- `1` Logowanie
- `2` Rejestracja
- `other` wylaczenie programu
### Logowanie
Przy wybraniu Logowania jetesmy proszeni o nadanych przez nas wcześniej nazw użytkownika i Hasła.
### Rejestracja
Przy tworzeniu nowego konda jesteśmy proszeni poproszeni o nadanie:
- Nazwy użytkownika która musi być o długości przynajmiej 3 znaków i może zawierać, tylko Litery, liczby i  znaki podkreślenia
- Hasła musi mieć co najmniej 6 znaków, zawierać co najmniej jedną wielką literę, jedną małą literę, jedną cyfrę i jeden znak specjalny
- Iminia naszego

## Panel użytkownika
Po zalogowaniu użytkownika, dostępne są opcje
- ## View Your Orders
    Po wybraniu tej opcji zostaną wyświetlone wszystkie zamówienia będące w trakcie realizacji. Każde zamówienie posiada unikalny numer.

    Użytkownik ma możliwość anulowania zamówienia poprzez wpisanie jego numeru. W przypadku naciśnięcia `ENTER` zostanie wyświetlone menu z pytaniem, czy chce wyświetlić historię zamówień:

    - Wciśnięcie `y` spowoduje wyświetlenie listy wszystkich zrealizowanych zamówień.
    - Wciśnięcie `n` spowoduje powrót do menu głównego.

- ## Place an Order
    Zostaną nam wyświetlone opcję
    - 1. Select from the Menu
    - 2. Create a Custom Pizza
    ### Select from the Menu
    Wyświetla menu pizz, w którym podane są: numer, nazwa, ceny dla poszczególnych rozmiarów oraz składniki każdej pizzy.  
    Następnie użytkownik jest proszony o podanie numeru pizzy, którą chce zamówić, a potem o wybór rozmiaru (S/M/L). Po dokonaniu wyboru użytkownik jest pytany, czy chce dodać kolejną pizzę:  
    - Wybór "y" pozwala na ponowne zamówienie pizzy.  
    - Wybór "n" powoduje powrót do strony głównej.


## Panel pracownika
Po zalogowaniu pracownika dostępne są następujące opcje:

### Zarządzanie zamówieniami
1. **Przeglądanie zamówień**  
   Po wybraniu tej opcji wyświetlone zostaną trzy podopcje:  
   - **1. Wyświetl aktywne zamówienia**  
     Wyświetla aktywne zamówienia wraz z ich identyfikatorem (ID), statusem i ceną.  
   - **2. Wyświetl historię zamówień**  
     Wyświetla wszystkie zrealizowane zamówienia, w tym informacje o osobie zamawiającej, ostatecznej cenie oraz zastosowanej promocji.  
   - **3. Wyświetl zamówienia użytkownika**  
     Wyświetla zamówienia dla wybranego użytkownika po podaniu jego nazwy użytkownika.  

2. **Aktualizacja statusu zamówienia**  
   Umożliwia zmianę statusu zamówienia po wprowadzeniu jego identyfikatora (ID).  

3. **Anulowanie zamówienia**  
   Umożliwia anulowanie zamówienia po wprowadzeniu jego identyfikatora (ID).  

### Wyświetlanie menu
 Wyświetla menu pizz, w którym podane są: numer, nazwa oraz cena bazowa każdej pizzy.  
 Naciśnięcie dowolnego przycisku powoduje powrót do głównego menu.


## Autorzy
- Jakub Borkowski, Antoni Zemło









