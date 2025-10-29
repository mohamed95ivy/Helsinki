// src/app/app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';

// Standalone features — import (not declare)
import { ConversionComponent } from './features/conversion/conversion.component';
import { HistoryComponent } from './features/history/history.component';
import { errorInterceptor } from './services/error-interceptor';
import { AppComponent } from './app';

@NgModule({
  declarations: [],            // AppComponent is a normal component
  imports: [
    BrowserModule,
    MatSnackBarModule,
    AppComponent
  ],
  providers: [
    provideHttpClient(withInterceptors([errorInterceptor]))
  ],
  bootstrap: []
})
export class AppModule {}
