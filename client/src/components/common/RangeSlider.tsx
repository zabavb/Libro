import React, { useState, useEffect, useRef } from 'react';
import '@/assets/styles/base/range-slider.css';

interface RangeSliderProps {
  min?: number;
  max?: number;
  value?: number;
  onChange?: (value: number) => void;
  isAudioOnly?: boolean; 
}

const RangeSlider: React.FC<RangeSliderProps> = ({
  min = 0,
  max = 100,
  value = 50,
  onChange,
  isAudioOnly = false, 
}) => {
  const [sliderValue, setSliderValue] = useState(value);
  const sliderRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    const percent = ((sliderValue - min) / (max - min)) * 100;
    if (sliderRef.current) {
      sliderRef.current.style.setProperty('--progress', `${percent}%`);
    }
  }, [sliderValue, min, max]);

  const handleInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = Number(e.target.value);
    setSliderValue(newValue);
    onChange?.(newValue);
  };

  return (
    <div
      className={`${
        isAudioOnly ? 'audio-only-slider' : ''
      }`} 
    >
      <div className="flex justify-between">
        <p className="price-limit">{min}</p>
        <p className="price-limit">{max}</p>
      </div>
      <div className="relative w-full h-6">
        <div className="thin-background" />

        <input
          ref={sliderRef}
          type="range"
          className={`thick-slider ${isAudioOnly ? 'audio-only' : ''}`} // Додаємо клас audio-only для зміни стилю
          min={min}
          max={max}
          value={sliderValue}
          onInput={handleInput}
        />
      </div>
    </div>
  );
};

export default RangeSlider;
