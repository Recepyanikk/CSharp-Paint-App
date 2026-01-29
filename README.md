# 🎨 Advanced GDI+ Drawing & Pixel Art Engine

![License](https://img.shields.io/badge/license-MIT-blue.svg) ![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg) ![Framework](https://img.shields.io/badge/framework-.NET%20Framework-purple.svg) ![Language](https://img.shields.io/badge/language-C%23-green.svg)

## 📖 Proje Hakkında
Bu proje, **System.Drawing (GDI+)** kütüphanesinin sınırlarını zorlayan, performans odaklı bir **Raster Grafik Editörü** mühendislik çalışmasıdır. Standart bir "Paint" uygulamasının ötesinde, bu yazılım **özel rendering algoritmaları**, **bellek güvenli katman yönetimi** ve **yığın (stack) tabanlı durum kontrolü** gibi gelişmiş yazılım mimarisi tekniklerini sergilemektedir.

Amacı, WinForms üzerinde düşük seviyeli grafik manipülasyonunun ve piksel tabanlı işlemlerin nasıl optimize edilebileceğini göstermektir.

---

## 🚀 Teknik Mimari ve Mühendislik Detayları

Proje arka planda 4 ana teknik ayağa dayanmaktadır:

### 1. Optimize Edilmiş Rendering Motoru (Non-Blocking Painting)
Standart `MouseMove` olaylarında yaşanan "kesik çizgi" (gap) problemini çözmek için, nokta tabanlı çizim yerine **Vektörel Enterpolasyon** tekniği kullanılmıştır.
*   **Sorun:** Hızlı fare hareketlerinde, olay tetikleyici (trigger) hızı, imleç hızına yetişemez ve noktalar arasında boşluklar oluşur.
*   **Çözüm:** Önceki nokta ile mevcut nokta arasında `Graphics.DrawLine` metodu ve `LineCap.Round` kullanılarak dinamik birleştirme yapılır. Bu, işletim sistemi çizim sırasından bağımsız pürüzsüz antialiased çizgiler sağlar.

### 2. Yinelemeli (Iterative) Flood Fill Algoritması
Özyinelemeli (Recursive) algoritmaların büyük görsellerde neden olduğu **StackOverflowException** riskini elimine eden, kendi yığınını (Stack) yöneten bir `Scanline-like` algoritma geliştirilmiştir.

```csharp
// Örnek Kod: Yığın Tabanlı Flood Fill (Form1.cs)
private void Renk_Doldur(Bitmap bmp, Point pt, Color hedefRenk, Color yeniRenk)
{
    if (hedefRenk.ToArgb() == yeniRenk.ToArgb()) return;

    Stack<Point> pixels = new Stack<Point>(); // Sistem Stack'i yerine Heap kullanıyoruz
    pixels.Push(pt);

    while (pixels.Count > 0)
    {
        Point a = pixels.Pop();
        // 4-Way Connectivity Kontrolü ve Renk Atama
        // ...
    }
}
```

### 3. Kayıpsız Geri Al (Undo) Mimarisi ve Bellek Yönetimi
Geri alma işlemleri için `Stack<Bitmap>` veri yapısı kullanılarak "Snapshot" (anlık görüntü) deseni uygulanmıştır.
*   **Memory Leak Prevention:** .NET'in Garbage Collector (GC) mekanizmasına tam güvenilmemiş, her geri alma işleminde veya yeni çizimde eski grafik nesneleri için manuel `Dispose()` çağrıları yapılarak GDI+ kaynakları (Handles) anında serbest bırakılmıştır.
*   **Limit:** Bellek şişmesini önlemek için yığın boyutu kontrol altına alınmıştır (Soft-Limit).

### 4. Sanal Izgara (Virtual Grid Layer)
Pixel Art modunda kullanıcıya yardımcı olan ızgara sistemi, ana tuval (Canvas) üzerine değil, `OnPaint` override edilerek sanal bir katman olarak çizilir.
*   Export edildiğinde ızgara resme dahil edilmez.
*   `Invalidate()` çağrıları ile sadece değişen bölgeler render edilerek CPU kullanımı düşürülmüştür.

---

## 🛠️ Teknoloji Yığını

| Bileşen | Teknoloji | Açıklama |
| :--- | :--- | :--- |
| **Core** | C# / .NET 4.7.2+ | Strong-Typed ve Object Oriented mimari |
| **UI Framework** | WinForms | Native Windows kontrolleri ve Event-Driven yapı |
| **Graphics API** | System.Drawing (GDI+) | Düşük seviyeli 2D grafik işlemleri |
| **Data Structures** | Generics (Stack, List) | Durum yönetimi ve algoritma optimizasyonu |
| **File I/O** | System.IO / ImageFormat | Asenkron dosya okuma/yazma ve stream yönetimi |

---

## 📊 Geliştirme Metrikleri

Proje, çevik (agile) prensipler izlenerek modüler bir yapıda geliştirilmiştir.

*   **Toplam Geliştirme Süresi:** 16 Saat (Sprint)
*   **Algoritma Optimizasyonu:** %25 Zaman Dilimi
*   **UI/UX Entegrasyonu:** %35 Zaman Dilimi
*   **Core Logic & Refactoring:** %40 Zaman Dilimi

---

## 💻 Kurulum ve Test

Projeyi yerel ortamınızda çalıştırmak ve kaynak kodları incelemek için:

1.  **Repoyu Klonlayın:**
    ```bash
    git clone https://github.com/Recepyanikk/CSharp-Paint-App.git
    ```
2.  **Projeyi Açın:** `.sln` dosyasını Visual Studio 2019/2022 ile açın.
3.  **Build:** Solution Explorer üzerinden `Build Solution` (Ctrl+Shift+B) diyerek exe'yi oluşturun.
4.  **Çalıştırın:** F5 ile debug modunda başlatın.

---

## 🔮 Gelecek Planları (Roadmap)

*   [ ] **Layer (Katman) Yönetimi:** Photoshop benzeri çoklu katman desteği.
*   [ ] **Vector Export:** Çizimlerin SVG formatında dışa aktarımı.
*   [ ] **Filtreler:** Parlaklık, Kontrast ve Blur efektleri için matris işlemleri.

---
*Developed by [Recep Yanık]*
