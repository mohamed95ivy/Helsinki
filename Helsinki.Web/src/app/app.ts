import { Component, effect, signal, OnInit } from '@angular/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatIconModule } from '@angular/material/icon';
import { HistoryComponent } from './features/history/history.component';
import { ConversionComponent } from './features/conversion/conversion.component';
import { ThemeService } from './services/theme-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrls: ['./app.css'],
  standalone: true, // If you're using standalone components
  imports: [MatSlideToggleModule, MatIconModule, HistoryComponent, ConversionComponent] // Add this for standalone
})
export class AppComponent implements OnInit {
  dark = signal(false);

  constructor(private themeService: ThemeService) {
    themeService.init();

    effect(() => {
      this.themeService.set(this.dark());
    });
  }

  ngOnInit() {
    const saved = localStorage.getItem('darkMode');
    if (saved) {
      this.dark.set(saved === 'true');
    } else {
      const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
      this.dark.set(prefersDark);
    }
  }
}