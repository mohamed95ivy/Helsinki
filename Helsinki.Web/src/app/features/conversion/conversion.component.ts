import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { AsyncPipe, CommonModule, NgFor, NgIf } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-conversion',
  standalone: true,
  imports: [
    ReactiveFormsModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatSelectModule, 
    MatButtonModule, 
    CommonModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatIconModule
  ],
    
  templateUrl: './conversion.component.html'
})
export class ConversionComponent {
  private fb = inject(FormBuilder);
  private api = inject(ApiService);
  // TODO: Add Event to invoke on conversion. Reload history data accordingly.

  currencies = signal<string[]>([]);
  loading = signal(false);
  result = signal<{ amount: number; rate: number; to: string } | null>(null);


  form = this.fb.group({
    amount: [100, [Validators.required, Validators.min(0)]],
    from: ['EUR', [Validators.required, Validators.minLength(3), Validators.maxLength(3)]],
    to: ['USD', [Validators.required, Validators.minLength(3), Validators.maxLength(3)]]
  });


  ngOnInit() {
    this.api.getCurrencies().subscribe(list => this.currencies.set(list));
  }
swap(): void {
  const { from, to } = this.form.value;
  this.form.patchValue({ from: to, to: from });

  // optional: re-run conversion after swapping
  if (this.form.valid) {
    this.convert();
  }
}

  convert() {
    if (this.form.invalid) return;
    this.loading.set(true);
    const { amount, from, to } = this.form.value as any;
    this.api.convert({ fromCurrency: from, toCurrency: to, amount, userId: 'candidate' })
      .subscribe({
        next: r => {
          this.result.set({ amount: r.toAmount, rate: r.exchangeRate, to: r.toCurrency });
          this.loading.set(false);
        },
        error: _ => this.loading.set(false)
      });
  }
}