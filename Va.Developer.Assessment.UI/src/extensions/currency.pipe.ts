import { computed, type Ref } from 'vue';

export function useCurrency(price: number | Ref<number>, locale = 'en-ZA', currency = 'ZAR') {
  const formatPrice = (value: number) => {
    return new Intl.NumberFormat(locale, {
      style: 'currency',
      currency,
    }).format(value);
  };

  const formattedPrice = computed(() => formatPrice(typeof price === 'number' ? price : price.value));

  return { formattedPrice, formatPrice };
}
