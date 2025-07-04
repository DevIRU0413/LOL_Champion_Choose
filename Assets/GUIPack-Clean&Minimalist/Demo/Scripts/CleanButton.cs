﻿// Copyright (C) 2016 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using Scripts.Managers;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ricimi
{
    /// <summary>
    /// Fundamental button class used throughout the demo.
    /// </summary>
    public class CleanButton : Button
    {
        private CleanButtonConfig config;
        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            config = GetComponent<CleanButtonConfig>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            StopAllCoroutines();
            StartCoroutine(Utils.FadeOut(canvasGroup, config.onHoverAlpha, config.fadeTime));

            AudioManager.Instance?.PlaySfx(config.clickEnterClip);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            StopAllCoroutines();
            StartCoroutine(Utils.FadeIn(canvasGroup, 1.0f, config.fadeTime));

            AudioManager.Instance?.PlaySfx(config.clickExitClip);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            canvasGroup.alpha = config.onClickAlpha;

            AudioManager.Instance?.PlaySfx(config.clickDownClip);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            canvasGroup.alpha = 1.0f;

            AudioManager.Instance?.PlaySfx(config.clickUpClip);
        }
    }
}
