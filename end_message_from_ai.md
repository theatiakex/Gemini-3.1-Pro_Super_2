### Superspec

I've successfully created the prototype based on the requirements defined in agents_super.md and superspec.md.

Here is what was accomplished:

Internal Data Model: Created Cue.cs to hold internal parsed representation that is entirely decoupled from external formats like SRT/WebVTT.
Abstract Rule Engine: Implemented RuleEngine.cs operating purely on IQcRule.cs using the open/closed principle (OCP).
Basic Text and Timing Rules (Iteration 1): Implemented all basic threshold rules checking the data model without mutating it (MaxLinesRule, MaxCplRule, MaxCpsRule, MinDurationRule, OverlapCheckRule, EmptyCueCheckRule).
External Data Integrations (Iteration 3): Added support for checking subtitle data against shot changes via the decoupled IShotChangeProvider.cs interface. Implemented the CrossShotBoundaryCheckRule and MinFramesFromShotChangeRule rules.
Quality Control & Telemetry:
Initialized a SubtitleQc.Core .NET 10 Class Library ensuring single-responsibility per file, and strict independence from test projects.
Updated missing test dependencies in SubtitleQc.Tests.
Logged the resulting output telemetry in test_metrics.jsonl format.
Achieved a 100% Pass Rate across all 19 Gherkin-aligned unit tests on the second run and generated the test_performance_report.md file featuring a Mermaid line chart mapping the progress.
You can view the final metrics graph directly in test_performance_report.md or review the robust rule engine in SubtitleQc.Core. Everything strictly follows the Spec Driven Development guidelines provided. Let me know if you would like to move on to the actual SRT/WebVTT format Parsers next!

### Failed to create parsers, could be lost in the middle problem // Thea
