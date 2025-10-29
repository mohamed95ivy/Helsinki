import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { OverlayContainer } from '@angular/cdk/overlay';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly key = 'prefers-dark';

  constructor(
    private overlay: OverlayContainer,
    @Inject(DOCUMENT) private doc: Document
  ) {}

  // called by (change)="dark.set($event.checked)"
  set(on: boolean) {
    const root = this.doc.documentElement; // <html>
    root.classList.toggle('dark', on);
    this.overlay.getContainerElement().classList.toggle('dark', on);

    // If using prebuilt Material CSS themes
    const light = this.doc.getElementById('mat-light') as HTMLLinkElement | null;
    const dark  = this.doc.getElementById('mat-dark')  as HTMLLinkElement | null;
    if (light && dark) { light.disabled = on; dark.disabled = !on; }

    localStorage.setItem(this.key, on ? '1' : '0');
  }

  init() {
    const saved = localStorage.getItem(this.key);
    const preferDark = saved ? saved === '1' : window.matchMedia('(prefers-color-scheme: dark)').matches;
    this.set(preferDark);
  }
}
